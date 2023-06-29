
Unity URP and HDRP projects require shaders built for that specific render pipeline.

When the shader does not match the render pipeline the material will render in pink.

The EasyRoads3D Pro package includes separate material packages for URP and HDRP projects. These packages are located in:

/Assets/EasyRoads3D/SRP Support packages/ 

After the EasyRoads3D Pro package is imported, scripts run that will automatically import the required material package.

Should there still be pink materials, then try to manually import the specific package. This can be done by selecting the package from inside the Unity editor in the project window at the above mentioned path.
  
Please contact us should you still have material issues after that.

Forum: http://forum.unity3d.com/threads/easyroads3d-v3-the-upcoming-new-road-system.229327/
Website: http://www.easyroads3d.com
Support: info@easyroads3d.com