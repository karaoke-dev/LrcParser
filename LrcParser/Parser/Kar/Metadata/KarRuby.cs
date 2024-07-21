// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser.Kar.Metadata;

/// <summary>
/// Ruby tag
/// </summary>
/// <example>
/// @Ruby1=帰,かえ
/// @Ruby25=時,じか,,[00:38:45]
/// @Ruby49=時,とき,[00:38:45],[01:04:49]
/// </example>
public struct KarRuby : IEquatable<KarRuby>
{
    public KarRuby()
    {
    }

    /// <summary>
    /// Parent kanji
    /// </summary>
    public string Parent { get; set; } = string.Empty;

    /// <summary>
    /// Ruby
    /// </summary>
    public string Ruby { get; set; } = string.Empty;

    /// <summary>
    /// Time tags
    /// </summary>
    public SortedDictionary<TextIndex, int> TimeTags { get; set; } = new();

    /// <summary>
    /// Start position
    /// </summary>
    public int? StartTime { get; set; } = null;

    /// <summary>
    /// End position
    /// </summary>
    public int? EndTime { get; set; } = null;

    public bool Equals(KarRuby other)
    {
        return Parent == other.Parent
               && Ruby == other.Ruby
               && TimeTags.SequenceEqual(other.TimeTags)
               && StartTime == other.StartTime
               && EndTime == other.EndTime;
    }
}
