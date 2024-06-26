﻿using linkApi.DataContext;
using linkApi.Entities;
using linkApi.Factories;
using linkApi.Interfaces;
using Microsoft.Extensions.Configuration;

namespace linkApi.Tests;

public class FactoryTests
{
    [Fact]
    public void NormalizeUrlTest()
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


}