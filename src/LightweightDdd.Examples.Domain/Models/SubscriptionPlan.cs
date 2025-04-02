// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Models
{
    public sealed record SubscriptionPlan
    {
        private SubscriptionPlan(string name, int maxGalleryImages)
        {
            Name = name;
            MaxGalleryImages = maxGalleryImages;
        }

        public string Name { get; }
        public int MaxGalleryImages { get; }

        public static readonly SubscriptionPlan Free = new(nameof(Free), 3);

        public static readonly SubscriptionPlan Pro = new(nameof(Pro), 10);

        public static readonly SubscriptionPlan Unlimited = new(nameof(Unlimited), int.MaxValue);
    }

}
