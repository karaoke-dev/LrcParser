// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Parser.Lines;

public interface ISingleLineParser
{
    bool CanDecode(string text);

    bool CanEncode(object component);

    object Decode(string text);

    string Encode(object component, int index);
}
