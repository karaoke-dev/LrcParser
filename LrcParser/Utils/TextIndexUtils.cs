// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Utils;

internal static class TextIndexUtils
{
    internal static int ToStringIndex(TextIndex index)
        => GetValueByState(index, index.Index, index.Index + 1);

    internal static T GetValueByState<T>(TextIndex index, T startValue, T endValue) =>
        index.State switch
        {
            IndexState.Start => startValue,
            IndexState.End => endValue,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
}
