using System;

namespace AutoFilter;

[AttributeUsage(AttributeTargets.Property)]
public class NotAutoFilteredAttribute : Attribute
{
}