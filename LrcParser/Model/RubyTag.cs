// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Model;

public class RubyTag
{
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Time tags
    /// </summary>
    public SortedDictionary<TextIndex, int?> TimeTags { get; set; } = new();

    /// <summary>
    /// Start char index at <see cref="Lyric.Text"/>
    /// </summary>
    public int StartCharIndex { get; set; }

    /// <summary>
    /// Start char index at <see cref="Lyric.Text"/>
    /// </summary>
    public int EndCharIndex { get; set; }
}
