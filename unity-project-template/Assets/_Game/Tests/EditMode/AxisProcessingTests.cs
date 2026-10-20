using NUnit.Framework;
using SolDeCobre.Gameplay;

namespace SolDeCobre.Tests
{
    public sealed class AxisProcessingTests
    {
        const float Tol = 1e-4f;

        [Test] public void BelowDeadzoneIsZero()
        {
            Assert.AreEqual(0f, AxisProcessing.ApplyDeadzone(0.10f, 0.15f), Tol);
            Assert.AreEqual(0f, AxisProcessing.ApplyDeadzone(0.15f, 0.15f), Tol);
            Assert.AreEqual(0f, AxisProcessing.ApplyDeadzone(-0.14f, 0.15f), Tol);
        }
        [Test] public void FullDeflectionStaysOne()
        {
            Assert.AreEqual(1f, AxisProcessing.ApplyDeadzone(1f, 0.15f), Tol);
            Assert.AreEqual(-1f, AxisProcessing.ApplyDeadzone(-1f, 0.15f), Tol);
        }
        [Test] public void RescalesAboveDeadzonePreservingSign()
        {
            // (0.575 - 0.15) / (1 - 0.15) = 0.5
            Assert.AreEqual(0.5f, AxisProcessing.ApplyDeadzone(0.575f, 0.15f), Tol);
            Assert.AreEqual(-0.5f, AxisProcessing.ApplyDeadzone(-0.575f, 0.15f), Tol);
        }
        [Test] public void ClampsOutOfRangeInput()
        {
            Assert.AreEqual(1f, AxisProcessing.ApplyDeadzone(2f, 0.15f), Tol);
            Assert.AreEqual(-1f, AxisProcessing.ApplyDeadzone(-9f, 0.15f), Tol);
        }
        [Test] public void NaNAndZeroDeadzoneAreSafe()
        {
            Assert.AreEqual(0f, AxisProcessing.ApplyDeadzone(float.NaN, 0.15f), Tol);
            Assert.AreEqual(0.42f, AxisProcessing.ApplyDeadzone(0.42f, 0f), Tol);
        }
    }
}
