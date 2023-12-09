// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Utils;
using NUnit.Framework;

namespace LrcParser.Tests.Utils;

public class TextIndexUtilsTest
{
    [TestCase(0, IndexState.Start, 0)]
    [TestCase(1, IndexState.End, 1)]
    [TestCase(-1, IndexState.Start, -1)] // In utils not checking is index out of range
    [TestCase(0, IndexState.End, 0)]
    public void TestToCharIndex(int index, IndexState state, int expected)
    {
        var textIndex = new TextIndex(index, state);

        int actual = TextIndexUtils.ToCharIndex(textIndex);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(0, IndexState.Start, 0)]
    [TestCase(0, IndexState.End, 1)]
    [TestCase(-1, IndexState.Start, -1)] // In utils not checking is index out of range
    [TestCase(-1, IndexState.End, 0)]
    public void TestToGapIndex(int index, IndexState state, int expected)
    {
        var textIndex = new TextIndex(index, state);

        int actual = TextIndexUtils.ToGapIndex(textIndex);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(IndexState.Start, 1, -1, 1)]
    [TestCase(IndexState.End, 1, -1, -1)]
    [TestCase(IndexState.Start, "start", "end", "start")]
    [TestCase(IndexState.End, "start", "end", "end")]
    public void TestGetValueByState(IndexState state, object startValue, object endValue, object expected)
    {
        var textIndex = new TextIndex(0, state);

        object actual = TextIndexUtils.GetValueByState(textIndex, startValue, endValue);
        Assert.That(actual, Is.EqualTo(expected));
    }
}
