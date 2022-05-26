// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser.Lrc.Metadata;

/// <summary>
/// Ruby tag
/// </summary>
/// <example>
/// @Ruby1=帰,かえ
/// @Ruby25=時,じか,,[00:38:45]
/// @Ruby49=時,とき,[00:38:45],[01:04:49]
/// </example>
public class LrcRuby
{
    /// <summary>
    /// Parent kanji
    /// </summary>
    public string Parent { get; set; } = "";

    /// <summary>
    /// Ruby
    /// </summary>
    public string Ruby { get; set; } = "";

    /// <summary>
    /// Time tags
    /// </summary>
    public SortedDictionary<TextIndex, int> TimeTags { get; set; } = new();

    /// <summary>
    /// Start position
    /// </summary>
    public int? StartTime { get; set; }

    /// <summary>
    /// End position
    /// </summary>
    public int? EndTime { get; set; }
}
