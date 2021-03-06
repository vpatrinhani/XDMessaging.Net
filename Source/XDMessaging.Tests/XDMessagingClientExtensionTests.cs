﻿using NUnit.Framework;

namespace XDMessaging.Tests
{
    [TestFixture]
    public class XDMessagingClientExtensionTests
    {
        [Test]
        public void GivenAmazonBroadcastImplThenShouldResolveSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Broadcasters.GetAmazonBroadcaster();

            // assert
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GivenAmazonListenerImplThenShouldResolveInstanceSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Listeners.GetAmazonListener();

            // assert
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GivenIoStreamBroadcastImplThenShouldResolveSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Broadcasters.GetIoStreamBroadcaster();

            // assert
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GivenIoStreamListenerImplThenShouldResolveSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Listeners.GetIoStreamListener();

            // assert
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GivenWindowsMessagingBroadcastImplThenShouldResolveSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Broadcasters.GetWindowsMessagingBroadcaster();

            // assert
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GivenWindowsMessagingListenerImplThenShouldResolveInstanceSuccess()
        {
            // arrange
            var client = new XDMessagingClient();

            // act
            var instance = client.Listeners.GetWindowsMessagingListener();

            // assert
            Assert.That(instance, Is.Not.Null);
        }
    }
}