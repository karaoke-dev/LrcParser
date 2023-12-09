// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Utils;

internal static class TextIndexUtils
{
    /// <summary>
    /// Convert text index to char index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    internal static int ToCharIndex(TextIndex index)
        => index.Index;

    /// <summary>
    /// Convert the text index to the index of the indicator.
    /// </summary>
    /// <example>
    /// [0]カ[1]ラ[2]オ[3]ケ[4]
    /// </example>
    /// <param name="index"></param>
    /// <returns></returns>
    internal static int ToGapIndex(TextIndex index)
        => GetValueByState(index, index.Index, index.Index + 1);

    /// <summary>
    /// Get the value by state.
    /// If the state is start, then return start value.
    /// If the state is end, then return end value.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="startValue"></param>
    /// <param name="endValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static T GetValueByState<T>(TextIndex index, T startValue, T endValue) =>
        index.State switch
        {
            IndexState.Start => startValue,
            IndexState.End => endValue,
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };
}
