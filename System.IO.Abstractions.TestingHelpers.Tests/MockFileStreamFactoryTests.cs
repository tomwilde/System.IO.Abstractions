﻿using System.Collections.Generic;
using NUnit.Framework; 

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileStreamFactoryTests
    {
        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_CreateForExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\existing.txt", MockFileData.NullObject }
            });

            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(@"c:\existing.txt", fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Append)]
        public void MockFileStreamFactory_CreateForNonExistingFile_ShouldReturnStream(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Act
            var result = fileStreamFactory.Create(XFS.Path(@"c:\not_existing.txt"), fileMode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(FileMode.Create)]
        [TestCase(FileMode.Open)]
        public void MockFileStreamFactory_CreateInNonExistingDirectory_ShouldThrowDirectoryNotFoundException(FileMode fileMode)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"C:\Test"));

            // Act
            var fileStreamFactory = new MockFileStreamFactory(fileSystem);

            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => fileStreamFactory.Create(@"C:\Test\NonExistingDirectory\some_random_file.txt", fileMode));
        }
    }
}