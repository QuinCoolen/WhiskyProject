using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WhiskyMVC.Controllers;
using WhiskyBLL.Services;
using WhiskyBLL.Dto;
using System.Collections.Generic;
using WhiskyBLL;
using System;
using WhiskyMVC.Models;
using WhiskyBLL.Interfaces;

[TestFixture]
public class WhiskyControllerTests : IDisposable
{
    private Mock<IWhiskyRepository> _whiskyRepositoryMock;
    private WhiskyService _whiskyService;
    private WhiskyController _whiskyController;

    [SetUp]
    public void Setup()
    {
        _whiskyRepositoryMock = new Mock<IWhiskyRepository>();
        _whiskyService = new WhiskyService(_whiskyRepositoryMock.Object);
        _whiskyController = new WhiskyController(_whiskyService, null);
    }

    public void Dispose()
    {
        _whiskyController?.Dispose();
    }

    [Test]
    public void Index_ShouldReturnViewWithWhiskys()
    {
        // Arrange
        var whiskys = new List<WhiskyDto>
        {
            new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" }
        };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskys()).Returns(whiskys);

        // Act
        var result = _whiskyController.Index() as ViewResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Model, Is.InstanceOf<List<WhiskyViewModel>>());
        Assert.That(((List<WhiskyViewModel>)result.Model).Count, Is.EqualTo(1));
    }

    [Test]
    public void Create_ShouldRedirectToIndex_WhenWhiskyIsCreated()
    {
        // Arrange
        var whiskyViewModel = new WhiskyViewModel { Name = "Whisky D", Age = 8, Year = 2015, Country = "Scotland", Region = "Islay" };

        // Act
        var result = _whiskyController.Create(whiskyViewModel) as RedirectToActionResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void Delete_ShouldRedirectToIndex_WhenWhiskyIsDeleted()
    {
        // Arrange
        var whiskyId = 1;
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyById(whiskyId)).Returns(new WhiskyDto { Id = whiskyId });

        // Act
        var result = _whiskyController.Delete(whiskyId) as RedirectToActionResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }
} 