namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for Cameras
    /// </summary>
    public static class CameraExtensions
    {
        /// <summary>
        /// Returns the viewport of the camera as a Rect.  This method is intended for Orthographic cameras.
        /// </summary>
        /// <param name="camera">The camera to get the viewport of</param>
        /// <returns>The viewport of the camera</returns>
        public static Rect GetViewport(this Camera camera)
        {
            var bottomLeftPoint = camera.ScreenToWorldPoint(Vector3.zero);
            var topRightPoint = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
            var viewportRectangle = new Rect(bottomLeftPoint.x, bottomLeftPoint.y, topRightPoint.x - bottomLeftPoint.x, topRightPoint.y - bottomLeftPoint.y);
            return viewportRectangle;
        }
    }
}