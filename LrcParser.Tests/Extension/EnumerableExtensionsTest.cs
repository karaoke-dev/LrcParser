// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using LrcParser.Extension;
using NUnit.Framework;

namespace LrcParser.Tests.Extension;

public class EnumerableExtensionsTest
{
    [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [TestCase(new[] { 1, 1, 1 }, new[] { 1 })]
    [TestCase(new[] { 1, 1, 2, 2, 1, 1, 2, 3 }, new[] { 1, 2, 1, 2, 3 })]
    public void TestGroupByContinuous(int[] values, int[] expected)
    {
        var groups = values.GroupByContinuous(x => x);
        Assert.AreEqual(expected, groups.Select(x => x.Key));
    }

    [TestCase(new[] { "A", "B", "C" }, new[] { "A", "B", "C" })]
    [TestCase(new[] { "A", "A", "A" }, new[] { "A" })]
    public void TestGroupByContinuous(string[] values, string[] expected)
    {
        var groups = values.GroupByContinuous(x => x);
        Assert.AreEqual(expected, groups.Select(x => x.Key));
    }
}
