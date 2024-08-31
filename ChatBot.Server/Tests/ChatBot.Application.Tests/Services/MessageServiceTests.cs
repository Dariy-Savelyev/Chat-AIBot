using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.Services;
using ChatBot.CrossCutting.Exceptions;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Moq;
using System.Linq.Expressions;

namespace ChatBot.Application.Tests.Services;

public class MessageServiceTests
{
    private readonly Mock<IMessageRepository> _mockMessageRepository;
    private readonly Mock<IChatRepository> _mockChatRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        _mockMessageRepository = new Mock<IMessageRepository>();
        _mockChatRepository = new Mock<IChatRepository>();
        _mockMapper = new Mock<IMapper>();
        _messageService = new MessageService(_mockMessageRepository.Object, _mockChatRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ValidUserAndChat_ReturnsMessageId()
    {
        // Arrange
        var model = new HubAddMessageModel();
        var userId = "user1";
        var message = new Message { Id = 1, ChatId = 1 };
        var chat = new Chat { Id = 1, Users = [new() { Id = userId }] };

        _mockMapper.Setup(m => m.Map<Message>(model)).Returns(message);
        _mockChatRepository.Setup(r => r.GetByIdAsync(message.ChatId, It.IsAny<Expression<Func<Chat, ICollection<User>>>>()))
            .ReturnsAsync(chat);
        _mockMessageRepository.Setup(r => r.AddAsync(message)).Returns(Task.CompletedTask);

        // Act
        var result = await _messageService.SendMessageAsync(model, userId);

        // Assert
        Assert.Equal(1, result);
        _mockMessageRepository.Verify(r => r.AddAsync(message), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_UnauthorizedUser_ThrowsForbiddenException()
    {
        // Arrange
        var model = new HubAddMessageModel();
        var userId = "user1";
        var message = new Message { Id = 1, ChatId = 1 };
        var chat = new Chat { Id = 1, Users = [new() { Id = "user2" }] };

        _mockMapper.Setup(m => m.Map<Message>(model)).Returns(message);
        _mockChatRepository.Setup(r => r.GetByIdAsync(message.ChatId, It.IsAny<Expression<Func<Chat, ICollection<User>>>>()))
            .ReturnsAsync(chat);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _messageService.SendMessageAsync(model, userId));
    }

    [Fact]
    public async Task GetAllMessagesAsync_ReturnsMessagesForChat()
    {
        // Arrange
        var chatId = 1;

        var messages = new List<Message>
            {
                new() { Id = 1, ChatId = chatId },
                new() { Id = 2, ChatId = chatId },
                new() { Id = 3, ChatId = 2 }
            };

        var expectedMessages = new List<GetAllMessageModel>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };

        _mockMessageRepository.Setup(r => r.GetAllAsync(false)).ReturnsAsync(messages);
        _mockMapper.Setup(m => m.Map<IEnumerable<GetAllMessageModel>>(It.IsAny<IEnumerable<Message>>()))
            .Returns(expectedMessages);

        // Act
        var result = await _messageService.GetAllMessagesAsync(chatId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(expectedMessages, result);
    }

    [Fact]
    public async Task SetEmoteAsync_CallsRepositoryMethod()
    {
        // Arrange
        var model = new MessageEmoteModel { MessageId = 1, Emote = null };
        var message = new Message { Id = 1, Emote = null };

        _mockMapper.Setup(m => m.Map<Message>(model)).Returns(message);
        _mockMessageRepository.Setup(r => r.AddEmoteAsync(message.Id, message.Emote)).Returns(Task.CompletedTask);

        // Act
        await _messageService.SetEmoteAsync(model);

        // Assert
        _mockMessageRepository.Verify(r => r.AddEmoteAsync(message.Id, message.Emote), Times.Once);
    }
}