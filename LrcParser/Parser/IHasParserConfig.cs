// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Parser;

public interface IHasParserConfig<TEncodeConfig, TDecodeConfig>
    where TEncodeConfig : EncodeConfig
    where TDecodeConfig : DecodeConfig
{
    TEncodeConfig EncodeConfig { get; set; }
    TDecodeConfig DecodeConfig { get; set; }
}
