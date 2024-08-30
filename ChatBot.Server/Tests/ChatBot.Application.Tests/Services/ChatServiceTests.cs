using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.Services;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Moq;
using System.Linq.Expressions;

namespace ChatBot.Application.Tests.Services;

public class ChatServiceTests
{
    private readonly Mock<IChatRepository> _mockChatRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        _mockChatRepository = new Mock<IChatRepository>();
        _mockMapper = new Mock<IMapper>();
        _chatService = new ChatService(_mockChatRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateChatAsync_ShouldReturnChatId()
    {
        // Arrange
        var chatModel = new ChatModel();
        var chat = new Chat { Id = 1 };
        var userId = "user123";

        _mockMapper.Setup(m => m.Map<Chat>(chatModel)).Returns(chat);
        _mockChatRepository.Setup(r => r.AddAsync(It.IsAny<Chat>())).Returns(Task.CompletedTask);

        // Act
        var result = await _chatService.CreateChatAsync(chatModel, userId);

        // Assert
        Assert.Equal(1, result);
        Assert.Equal(userId, chat.CreatorId);
        _mockChatRepository.Verify(r => r.AddAsync(chat), Times.Once);
    }

    [Fact]
    public async Task JoinChatAsync_ShouldCallRepositoryMethod()
    {
        // Arrange
        var model = new JoinToChatModel { ChatId = 1 };
        var userId = "user123";

        _mockChatRepository.Setup(r => r.JoinUserAsync(userId, model.ChatId)).Returns(Task.CompletedTask);

        // Act
        await _chatService.JoinChatAsync(model, userId);

        // Assert
        _mockChatRepository.Verify(r => r.JoinUserAsync(userId, model.ChatId), Times.Once);
    }

    [Fact]
    public async Task GetAllChatsAsync_ShouldReturnOrderedAndMappedChats()
    {
        // Arrange
        var dbChats = new List<Chat>
            {
                new() { Id = 1, DateCreate = DateTime.Now.AddDays(-2) },
                new() { Id = 2, DateCreate = DateTime.Now },
                new() { Id = 3, DateCreate = DateTime.Now.AddDays(-1) }
            };

        var orderedChats = dbChats.OrderByDescending(x => x.DateCreate).ToList();

        var expectedChats = new List<GetAllChatModel>
            {
                new() { Id = 2 },
                new() { Id = 3 },
                new() { Id = 1 }
            };

        _mockChatRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Chat, ICollection<User>>>>()))
            .ReturnsAsync(dbChats);

        _mockMapper.Setup(m => m.Map<IEnumerable<GetAllChatModel>>(It.Is<IEnumerable<Chat>>(c =>
            c.First().Id == orderedChats.First().Id &&
            c.Last().Id == orderedChats.Last().Id)))
            .Returns(expectedChats);

        // Act
        var result = await _chatService.GetAllChatsAsync();

        // Assert
        Assert.Equal(expectedChats, result);
        Assert.Equal(2, result.First().Id);
        Assert.Equal(1, result.Last().Id);
        _mockChatRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Chat, ICollection<User>>>>()), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<GetAllChatModel>>(It.Is<IEnumerable<Chat>>(c =>            c.First().Id == orderedChats.First().Id && c.Last().Id == orderedChats.Last().Id)), Times.Once);
    }
}