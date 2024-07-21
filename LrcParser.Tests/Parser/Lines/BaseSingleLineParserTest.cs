// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using LrcParser.Parser.Lines;

namespace LrcParser.Tests.Parser.Lines;

public class BaseSingleLineParserTest<TParser, TModel>
    where TParser : SingleLineParser<TModel>, new() where TModel : struct, IEquatable<TModel>
{
    protected bool CanDecode(string text) => new TParser().CanDecode(text);

    protected TModel Decode(string text) => new TParser().Decode(text);

    protected string Encode(TModel component) => new TParser().Encode(component, 0);
}
