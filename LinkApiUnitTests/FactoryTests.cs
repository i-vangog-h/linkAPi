﻿using linkApi.Factories;
using linkApi.Interfaces;

namespace linkApi.Tests;

public class FactoryTests
{
    [Fact]
    public void NormalizeTrailingSlashesUrlTest()
    {
        // arrange

        IUrlFactory _factory = new UrlFactory();

        string urlWithTrailingSlash  = "https://test.com/index";
        string expected = "https://test.com/index";

        //act

        string actual = _factory.Normalize(urlWithTrailingSlash);

        //assert

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NormalizeEmptySpacesPreceedingUrlTest()
    {
        // arrange

        IUrlFactory _factory = new UrlFactory();

        string urlWithTrailingSlash = "     https://test.com/index";
        string expected = "https://test.com/index";

        //act

        string actual = _factory.Normalize(urlWithTrailingSlash);

        //assert

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NormalizeEmptySpacesTrailingUrlTest()
    {
        // arrange

        IUrlFactory _factory = new UrlFactory();

        string urlWithTrailingSlash = "https://test.com/index      ";
        string expected = "https://test.com/index";

        //act

        string actual = _factory.Normalize(urlWithTrailingSlash);

        //assert

        Assert.Equal(expected, actual);
    }
}