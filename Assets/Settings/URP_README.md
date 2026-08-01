# Universal Render Pipeline settings

This project targets Unity 6.5 and newer with the Universal Render Pipeline (URP), because the Built-In Render Pipeline is deprecated beginning with Unity 6.5 and remains supported only through Unity 6.7.

After the first package restore, create or assign the project URP asset in Unity:

1. Open **Window > Package Management > Package Manager** and confirm **Universal RP** is installed.
2. Open **Edit > Project Settings > Graphics**.
3. Assign the project's Universal Render Pipeline Asset.
4. If Unity prompts for material conversion, run the URP converter for built-in materials.

Keep new visual assets and materials URP-compatible.

Unity 6 creates and assigns a URP Global Settings asset automatically when the
project is first opened. The corresponding console message is informational;
the generated asset can be inspected under **Project Settings > Graphics > URP
Global Settings Asset**.

Dynamic batching is disabled for every build target configured by this project.
Use GPU instancing on compatible materials when repeated meshes need to be
batched.
