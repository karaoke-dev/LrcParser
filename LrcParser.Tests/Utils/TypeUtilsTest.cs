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
        Assert.That(TypeUtils.ChangeType<string>("123"), Is.EqualTo("123"));

        // test number
        Assert.That(TypeUtils.ChangeType<int>(456), Is.EqualTo(456));

        // test another number
        Assert.That(TypeUtils.ChangeType<float>(789f), Is.EqualTo(789f));

        // test class, should use same instance.
        var testClass = new TestClass();
        Assert.That(TypeUtils.ChangeType<TestClass>(testClass), Is.EqualTo(testClass));
    }

    [Test]
    public void TestChangeTypeToDifferentType()
    {
        // test convert to number
        Assert.That(TypeUtils.ChangeType<double>(123), Is.EqualTo(Convert.ToDouble(123)));

        // test convert to string
        Assert.That(TypeUtils.ChangeType<string>(123), Is.EqualTo(Convert.ToString(123)));

        // test convert to nullable
        Assert.That(TypeUtils.ChangeType<double?>(123d), Is.EqualTo(123));
        Assert.That(TypeUtils.ChangeType<double?>(null), Is.EqualTo(default(double?)));
    }

    private class TestClass
    {
    }
}
