// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Parser.Lines;

/// <inheritdoc>
/// Base component pass string
/// </inheritdoc>
/// <typeparam name="T">Encode and decode object type</typeparam>
public abstract class SingleLineParser<T> : ISingleLineParser where T : struct, IEquatable<T>
{
    public abstract bool CanDecode(string text);

    public bool CanEncode(object component)
        => component is T;

    object ISingleLineParser.Decode(string text) => Decode(text);

    public string Encode(object component, int index)
    {
        if (component is not T target)
            throw new InvalidCastException();

        return Encode(target, index);
    }

    /// <summary>
    /// Decode to target class and leave remain text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public abstract T Decode(string text);

    /// <summary>
    /// Encode target component
    /// </summary>
    /// <param name="component"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public abstract string Encode(T component, int index);
}
