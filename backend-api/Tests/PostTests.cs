using Xunit;
using MonApiBackend.Models.Entities;

namespace MonApiBackend.Tests
{
    public class PostTests
    {
        [Fact]
        public void Post_DefaultValues_AreCorrect()
        {
            // Arrange
            var post = new Post();

            // Assert
            Assert.Equal("", post.Title);
            Assert.Equal("", post.Content);
            Assert.Equal(0, post.Upvotes);
            Assert.Equal(0, post.Downvotes);
            Assert.Equal("", post.Language);
            Assert.Equal("", post.Status);
            Assert.Equal(0, post.Views);
            Assert.True(post.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Post_WithValidData_PropertiesAreSet()
        {
            // Arrange
            var title = "Test Title";
            var content = "Test Content";
            var userId = 1;
            var language = "fr";

            // Act
            var post = new Post
            {
                Title = title,
                Content = content,
                UserId = userId,
                Language = language
            };

            // Assert
            Assert.Equal(title, post.Title);
            Assert.Equal(content, post.Content);
            Assert.Equal(userId, post.UserId);
            Assert.Equal(language, post.Language);
        }

        [Fact]
        public void Post_VoteCounters_WorkCorrectly()
        {
            // Arrange
            var post = new Post();

            // Act
            post.Upvotes = 5;
            post.Downvotes = 2;

            // Assert
            Assert.Equal(5, post.Upvotes);
            Assert.Equal(2, post.Downvotes);
        }
    }
} 