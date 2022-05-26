// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Model;

public class RubyTag
{
    public string Text { get; set; } = "";

    public int StartIndex { get; set; }

    public int EndIndex { get; set; }
}
