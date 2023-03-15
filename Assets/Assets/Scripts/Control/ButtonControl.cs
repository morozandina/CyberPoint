using UnityEngine.EventSystems;

namespace Assets.Scripts.Control
{
    public class ButtonControl : ButtonsControl
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            Rotation = buttonDirection switch
            {
                ButtonDirection.Left => 1,
                ButtonDirection.Right => -1,
                _ => Rotation
            };
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Rotation = 0;
        }
    }
}
