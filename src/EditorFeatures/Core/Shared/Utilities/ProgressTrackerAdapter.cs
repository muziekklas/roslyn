﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.Shared.Utilities;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.CodeAnalysis.Editor.Shared.Utilities
{
    internal class ProgressTrackerAdapter : IProgressTracker
    {
        private IUIThreadOperationScope _platformWaitScope;
        private int _completedItems;
        private int _totalItems;

        public ProgressTrackerAdapter(IUIThreadOperationScope platformWaitScope)
        {
            Requires.NotNull(platformWaitScope, nameof(platformWaitScope));
            _platformWaitScope = platformWaitScope;
        }

        public int CompletedItems => _completedItems;

        public int TotalItems => _totalItems;

        public void AddItems(int count)
        {
            Interlocked.Add(ref _totalItems, count);
            ReportProgress();
        }

        public void Clear()
        {
            Interlocked.Exchange(ref _completedItems, 0);
            Interlocked.Exchange(ref _totalItems, 0);
            ReportProgress();
        }

        public void ItemCompleted()
        {
            Interlocked.Increment(ref _completedItems);
            ReportProgress();
        }

        private void ReportProgress()
        {
            _platformWaitScope.Progress.Report(new ProgressInfo(_completedItems, _totalItems));
        }
    }
}
