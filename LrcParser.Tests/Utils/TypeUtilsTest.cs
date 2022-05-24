// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using LrcParser.Utils;
using NUnit.Framework;

namespace LrcParser.Tests.Utils;

public class TypeUtilsTest
{
    [Test]
    public void TestChangeTypeToSameType()
    {
        // test string
        Assert.AreEqual("123", TypeUtils.ChangeType<string>("123"));

        // test number
        Assert.AreEqual(456, TypeUtils.ChangeType<int>(456));

        // test another number
        Assert.AreEqual(789f, TypeUtils.ChangeType<float>(789f));

        // test class, should use same instance.
        var testClass = new TestClass();
        Assert.AreEqual(testClass, TypeUtils.ChangeType<TestClass>(testClass));
    }

    [Test]
    public void TestChangeTypeToDifferentType()
    {
        // test convert to number
        Assert.AreEqual(Convert.ToDouble(123), TypeUtils.ChangeType<double>(123));

        // test convert to string
        Assert.AreEqual(Convert.ToString(123), TypeUtils.ChangeType<string>(123));

        // test convert to nullable
        Assert.AreEqual(123, TypeUtils.ChangeType<double?>(123d));
        Assert.AreEqual(default(double?), TypeUtils.ChangeType<double?>(null));
    }

    private class TestClass
    {
    }
}
