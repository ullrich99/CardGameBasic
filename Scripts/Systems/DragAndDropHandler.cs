using Godot;
using Godot.Collections;
using System;

public partial class DragAndDropHandler : Node3D
{
    Node3D draggingCollider;
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

            if (intersect.Count > 0)
            {
                Variant f = new Variant();
                intersect.TryGetValue("position", out f);
                mousePosition = f.AsVector3();
                Variant c = new Variant();
                bool hasCollider = intersect.TryGetValue("collider", out c);
                if (hasCollider && doDrag)
                {
                    Node3D cs = (Node3D)c.AsGodotObject();

                    if (cs.IsInGroup("Card"))
                    {
                        draggingCollider = cs;
                        Node3D n = draggingCollider;
                        GD.Print("moving-" + n.Name);
                        float y = n.Position.Y;
                        n.Position = mousePosition;
                        Vector3 POS = n.Position;
                        POS.Y = y;
                        n.Position = POS;
                    }
                }
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
                if (draggingCollider != null)
                {
                    var pieces = GetTree().GetNodesInGroup("GroundPiece");
                    float closestPositionDistance = 200;
                    Vector3 closestPosition = draggingCollider.Position;
                    Vector3 POS = closestPosition;
                    foreach (var piece in pieces)
                    {

                        Node3D current = (Node3D)piece;
                        float distance = current.Position.DistanceTo(draggingCollider.Position);
                        GD.Print(piece.Name + distance);
                        if (distance < closestPositionDistance)
                        {
                            GD.Print(piece.Name + "_changed_" + draggingCollider.Name);
                            closestPositionDistance = distance;
                            closestPosition = current.Position;
                        }
                    }
                    closestPosition.Y = POS.Y;
                    draggingCollider.Position = closestPosition;
                    draggingCollider = null;

                }

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
        //Variant f = new Variant();
        //dic.TryGetValue("position", out f);
        //if(draggingCollider != null && doDrag)
        //{
        //    draggingCollider = dic;
        //}
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
