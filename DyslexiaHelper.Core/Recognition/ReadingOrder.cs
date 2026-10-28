using RapidOcrNet;

namespace DyslexiaHelper.Core.Recognition;

internal static class ReadingOrder
{
    private sealed record PlacedBlock(string Text, float Left, float Top, float Bottom)
    {
        public float CenterY => (Top + Bottom) / 2f;
        public float Height => Bottom - Top;
    }

    private static PlacedBlock Place(TextBlock block)
    {
        var points = block.BoxPoints;
        return new PlacedBlock(
            block.Text,
            points.Min(point => point.X),
            points.Min(point => point.Y),
            points.Max(point => point.Y));
    }

    private static bool IsOnSameLine(PlacedBlock lineStart, PlacedBlock candidate)
    {
        var tolerance = Math.Min(lineStart.Height, candidate.Height) / 2f;
        return Math.Abs(lineStart.CenterY - candidate.CenterY) <= tolerance;
    }

    public static string Join(IEnumerable<TextBlock> blocks)
    {
        var sortedBlocks = blocks.Select(Place).OrderBy(block => block.CenterY);
        var lines = new List<List<PlacedBlock>>();

        foreach(var block in sortedBlocks)
        {
            if(lines.Count > 0 && IsOnSameLine(lines[^1][0], block))
                lines[^1].Add(block);
            else
                lines.Add(new List<PlacedBlock> { block });
        }

        return string.Join("\n", lines.Select(line => string.Join(" ", line.OrderBy(block => block.Left).Select(block => block.Text))));
    }
}
