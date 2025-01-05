using NUnit.Framework;
using Moq;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using System.Collections.Generic;
using WhiskyBLL.Exceptions;

[TestFixture]
public class WhiskyServiceTests
{
    private Mock<IWhiskyRepository> _whiskyRepositoryMock;
    private WhiskyService _whiskyService;

    [SetUp]
    public void Setup()
    {
        _whiskyRepositoryMock = new Mock<IWhiskyRepository>();
        _whiskyService = new WhiskyService(_whiskyRepositoryMock.Object);
    }

    [Test]
    public void GetWhiskys_ShouldReturnListOfWhiskys()
    {
        // Arrange
        var whiskys = new List<WhiskyDto>
        {
            new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" },
            new WhiskyDto { Id = 2, Name = "Whisky B", Age = 12, Year = 2008, Country = "Scotland", Region = "Islay" }
        };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskys()).Returns(whiskys);

        // Act
        var result = _whiskyService.GetWhiskys();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Whisky A"));
    }

    [Test]
    public void CreateWhisky_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Name = "", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };

        // Act & Assert
        var ex = Assert.Throws<InvalidWhiskyDataException>(() => _whiskyService.CreateWhisky(whiskyDto));
        Assert.That(ex.Message, Is.EqualTo("Whisky name is required."));
    }

    [Test]
    public void CreateWhisky_ShouldAddWhisky_WhenDataIsValid()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Name = "Whisky C", Age = 15, Year = 2005, Country = "Scotland", Region = "Speyside" };

        // Act
        _whiskyService.CreateWhisky(whiskyDto);

        // Assert
        _whiskyRepositoryMock.Verify(repo => repo.CreateWhisky(It.IsAny<WhiskyDto>()), Times.Once);
    }

    [Test]
    public void CreateWhisky_ShouldThrowException_WhenWhiskyAlreadyExists()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyByName(whiskyDto.Name)).Returns(whiskyDto);

        // Act & Assert
        var ex = Assert.Throws<WhiskyAlreadyExistsException>(() => _whiskyService.CreateWhisky(whiskyDto));
        Assert.That(ex.Message, Is.EqualTo("Whisky already exists."));
    }

    [Test]
    public void EditWhisky_ShouldUpdateWhisky_WhenDataIsValid()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyById(whiskyDto.Id)).Returns(whiskyDto);

        var updatedWhiskyDto = new WhiskyDto { Id = 1, Name = "Whisky A Updated", Age = 12, Year = 2008, Country = "Scotland", Region = "Islay" };

        // Act
        _whiskyService.UpdateWhisky(updatedWhiskyDto);

        // Assert
        _whiskyRepositoryMock.Verify(repo => repo.UpdateWhisky(It.Is<WhiskyDto>(w => w.Name == "Whisky A Updated" && w.Age == 12 && w.Year == 2008)), Times.Once);
    }

    [Test]
    public void EditWhisky_ShouldThrowException_WhenWhiskyNotFound()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyById(whiskyDto.Id)).Returns((WhiskyDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _whiskyService.UpdateWhisky(whiskyDto));
        Assert.That(ex.Message, Is.EqualTo("Whisky not found."));
    }

    [Test]
    public void DeleteWhisky_ShouldDeleteWhisky_WhenWhiskyExists()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyById(whiskyDto.Id)).Returns(whiskyDto);

        // Act
        _whiskyService.DeleteWhisky(whiskyDto.Id);

        // Assert
        _whiskyRepositoryMock.Verify(repo => repo.DeleteWhisky(whiskyDto.Id), Times.Once);
    }

    [Test]
    public void DeleteWhisky_ShouldThrowException_WhenWhiskyNotFound()
    {
        // Arrange
        var whiskyDto = new WhiskyDto { Id = 1, Name = "Whisky A", Age = 10, Year = 2010, Country = "Scotland", Region = "Highlands" };
        _whiskyRepositoryMock.Setup(repo => repo.GetWhiskyById(whiskyDto.Id)).Returns((WhiskyDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _whiskyService.DeleteWhisky(whiskyDto.Id));
        Assert.That(ex.Message, Is.EqualTo("Whisky not found."));
    }
} 