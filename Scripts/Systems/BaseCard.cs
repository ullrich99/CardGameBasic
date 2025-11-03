using Godot;
using Godot.Collections;
using System;

public partial class BaseCard : Node3D
{
    CollisionObject3D draggingCollider;
    Vector3 mousePosition;
    bool doDrag = false;
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Input(InputEvent inputEvent)
    {
        var intersect = GetMouseIntersect((inputEvent as InputEventMouse).Position);
        if (inputEvent is InputEventMouse iem)
        {
            
            if(intersect.Count > 0)
            {
                Variant f = new Variant();
                intersect.TryGetValue("position",out f);
                mousePosition = f.AsVector3();
            }
            
            //if(intersect.)
        }
        if (inputEvent is InputEventMouseButton inputEventMouseButton) 
        {
            var leftButtonPressed = inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed;
            var leftButtonReleased = inputEventMouseButton.ButtonIndex == MouseButton.Left && !inputEventMouseButton.Pressed;
            if (leftButtonReleased)
            {
                doDrag = false;
                DragAndDrop(intersect);
                
            }
            else if (leftButtonPressed) 
            {
                doDrag = true;
                DragAndDrop(intersect);
            }
        }
    }

    public void DragAndDrop(Dictionary dic)
    {
        Variant f = new Variant();
        dic.TryGetValue("position", out f);
        if(draggingCollider != null && doDrag)
        {
            draggingCollider = dic.TryGetValue();
        }
    }

    public Dictionary GetMouseIntersect(Vector2 position) 
    {
        var currentCamera = GetViewport().GetCamera3D();
        var param = new PhysicsRayQueryParameters3D();
        param.From = currentCamera.ProjectRayOrigin(position);
        param.To = currentCamera.ProjectPosition(position, 1000);

        var worldspace = GetWorld3D().DirectSpaceState;
        var result = worldspace.IntersectRay(param);
        if (result.Count > 0)
        {
            GD.Print(result);
        }
        return result;

    } 
}
