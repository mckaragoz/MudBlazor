﻿using FluentAssertions;
using MudBlazor.Docs.Compiler;
using NUnit.Framework;

namespace MudBlazor.UnitTests.Docs.Utilities
{
    [TestFixture]
    public class MarkupCompilerTests
    {
        [Test]
        public void AttributePostprocessingTest1()
        {
            // pull out quotation marks
            var source = "<span class=\"htmlAttributeValue\">&quot;Some random value&quot;</span>";
            var actual = ExamplesMarkup.AttributePostprocessing(source);
            var expected = "<span class=\"quot\">&quot;</span><span class=\"htmlAttributeValue\">Some random value</span><span class=\"quot\">&quot;</span>";
            actual.Should().Be(expected);
        }

        [Test]
        public void AttributePostprocessingTest2()
        {
            // true, false
            var source = "<span class=\"htmlAttributeValue\">&quot;true&quot;</span>";
            var actual = ExamplesMarkup.AttributePostprocessing(source);
            var expected = "<span class=\"quot\">&quot;</span><span class=\"keyword\">true</span><span class=\"quot\">&quot;</span>";
            actual.Should().Be(expected);
        }

        [Test]
        public void AttributePostprocessingTest3()
        {
            // handle enums
            var source = "<span class=\"htmlAttributeValue\">&quot;Color.Primary&quot;</span>";
            var actual = ExamplesMarkup.AttributePostprocessing(source);
            var expected = "<span class=\"quot\">&quot;</span><span class=\"enum\">Color</span><span class=\"enumValue\">.Primary</span><span class=\"quot\">&quot;</span>";
            actual.Should().Be(expected);
        }
    }
}
