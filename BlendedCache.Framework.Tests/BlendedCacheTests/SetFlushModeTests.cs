using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;
using System.Reflection;
using NUnit.Framework;

namespace BlendedCache.Tests.BlendedCacheTests
{
	[TestFixture]
	public class SetFlushModeTests
	{
		private bool _initialFlushMode = false;
		private const string _flushModePrivateField = "_flushMode";
		private bool _flushMode = false;
		private bool _response = false;
		private ContextCacheMock _contextCacheMock;
		private string _removableCacheKey;

		[SetUp]
		public void SetUp()
		{
			_initialFlushMode = false;
			_flushMode = true;
			_response = false;

			_contextCacheMock = RMM.GeneratePartialMock<ContextCacheMock>();
			_removableCacheKey = "item";

			_contextCacheMock.Set(_removableCacheKey, new object());
		}

		[Test]
		public void turn_on_FlushMode_should_only_remove_prefixed_items()
		{
			Execute();

			RME.AssertWasCalled(_contextCacheMock, x => x.Remove(_removableCacheKey));
			RME.AssertWasCalled(_contextCacheMock, x => x.Remove(null), opt => opt.IgnoreArguments().Repeat.Once());
		}

		[Test]
		public void turn_on_FlushMode_when_initial_FlushMode_is_true_should_not_remove_anything()
		{
			_initialFlushMode = true;

			Execute();

			RME.AssertWasNotCalled(_contextCacheMock, x => x.Remove(null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void turn_on_FlushMode_when_initial_FlushMode_is_true_should_return_true()
		{
			_initialFlushMode = true;

			Execute();

			Assert.True(_response);
		}

		[Test]
		public void turn_on_FlushMode_when_initial_FlushMode_is_false_should_return_false()
		{
			_initialFlushMode = false;

			Execute();

			Assert.False(_response);
		}

		[Test]
		public void turn_off_FlushMode_should_only_remove_prefixed_items()
		{
			_flushMode = false;
			Execute();

			RME.AssertWasNotCalled(_contextCacheMock, x => x.Remove(null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void turn_off_FlushMode_when_initial_FlushMode_is_true_should_not_remove_anything()
		{
			_flushMode = false;
			_initialFlushMode = true;

			Execute();

			RME.AssertWasNotCalled(_contextCacheMock, x => x.Remove(null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void turn_off_FlushMode_when_initial_FlushMode_is_true_should_return_true()
		{
			_flushMode = false;
			_initialFlushMode = true;

			Execute();

			Assert.True(_response);
		}

		[Test]
		public void turn_off_FlushMode_when_initial_FlushMode_is_false_should_return_false()
		{
			_flushMode = false;
			_initialFlushMode = false;

			Execute();

			Assert.False(_response);
		}

		[Test]
		public void should_have_a_field_named_flush_mode()
		{
			var service = new BlendedCache<DataContext>(_contextCacheMock, null, null, null);
			var field = service.GetType().GetField(_flushModePrivateField, BindingFlags.NonPublic | BindingFlags.Instance);

			Assert.NotNull(field, "There is a field required to fully test out the flush mode behavior.");
		}

		private void Execute()
		{
			var service = new BlendedCache<DataContext>(_contextCacheMock, null, null, null);
			service.GetType().GetField(_flushModePrivateField, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(service, _initialFlushMode);

			_response = service.SetFlushMode(_flushMode);
		}

		private class DataContext
		{
		}
	}
}
