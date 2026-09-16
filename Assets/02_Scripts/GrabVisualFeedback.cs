using UnityEngine; // GameObject, Color, Renderer, Material 등 Unity 기본 기능을 사용한다.
using UnityEngine.XR.Interaction.Toolkit; // Hover·Select·Activate의 EventArgs를 사용한다.
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRGrabInteractable을 사용한다.

// 이 Script가 붙는 GameObject에 XR Grab Interactable이 반드시 있도록 한다.
// Component가 없다면 Unity가 자동으로 함께 추가한다.
[RequireComponent(typeof(XRGrabInteractable))]

// MonoBehaviour를 상속해야 이 C# class를 Unity Component로 사용할 수 있다.
public class GrabVisualFeedback : MonoBehaviour
{
    // [SerializeField]를 붙이면 private 변수도 Inspector에서 수정할 수 있다.
    // Color는 색상 값을 저장하는 자료형이다.
    [SerializeField] private Color normalColor = Color.cyan;       // 잡을 수 있는 기본 상태
    [SerializeField] private Color hoverColor = Color.yellow;     // Ray가 닿은 Hover 상태
    [SerializeField] private Color selectedColor = Color.green;   // Grip으로 잡은 Select 상태
    [SerializeField] private Color activatedColor = Color.magenta;// Trigger를 누른 Activate 상태

    // 아래 변수에는 실행 중 사용할 Component와 Material을 저장한다.
    // 아직 값을 넣지 않았으므로 선언만 해 둔다.
    private XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Material objectMaterial;

    // Awake는 Play Mode가 시작될 때 가장 먼저 한 번 실행된다.
    private void Awake()
    {
        // GetComponent<T>()는 같은 GameObject에서 T형 Component를 찾는다.
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectRenderer = GetComponent<Renderer>();

        // Renderer의 Material 복사본을 가져와 이 오브젝트의 색만 바꾸게 한다.
        objectMaterial = objectRenderer.material;

        // 시작할 때 기본 상태 색상인 하늘색을 적용한다.
        objectMaterial.color = normalColor;
    }

    // OnEnable은 GameObject가 활성화될 때 실행된다.
    private void OnEnable()
    {
        // AddListener는 XRI 상태가 변했을 때 실행할 함수를 연결한다.
        // 예: Hover가 시작되면 OnHoverEntered 함수를 실행한다.
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
        grabInteractable.activated.AddListener(OnActivated);
        grabInteractable.deactivated.AddListener(OnDeactivated);
    }

    // OnDisable은 GameObject가 비활성화될 때 실행된다.
    private void OnDisable()
    {
        // RemoveListener로 앞에서 연결한 함수를 해제한다.
        // 해제하지 않으면 같은 함수가 중복 실행될 수 있다.
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
        grabInteractable.activated.RemoveListener(OnActivated);
        grabInteractable.deactivated.RemoveListener(OnDeactivated);
    }

    // Ray가 오브젝트에 처음 닿으면 실행된다.
    // args에는 어떤 Interactor가 Hover했는지 등의 사건 정보가 들어 있다.
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        // 이미 잡고 있는 중이 아니라면 Hover 색상으로 바꾼다.
        if (!grabInteractable.isSelected)
            objectMaterial.color = hoverColor;
    }

    // Ray가 오브젝트에서 벗어나면 실행된다.
    private void OnHoverExited(HoverExitEventArgs args)
    {
        // 잡고 있는 중이 아니라면 기본 색상으로 돌아간다.
        if (!grabInteractable.isSelected)
            objectMaterial.color = normalColor;
    }

    // Grip 입력으로 오브젝트를 잡는 순간 실행된다.
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        objectMaterial.color = selectedColor;
    }

    // Grip 입력을 해제하여 오브젝트를 놓는 순간 실행된다.
    private void OnSelectExited(SelectExitEventArgs args)
    {
        // 놓은 뒤에도 Ray가 닿아 있으면 노란색, 아니면 하늘색을 사용한다.
        // 조건 ? 값1 : 값2는 조건에 따라 둘 중 하나를 선택하는 삼항 연산자다.
        objectMaterial.color =
            grabInteractable.isHovered ? hoverColor : normalColor;
    }

    // 잡은 상태에서 Trigger를 눌러 Activate가 시작되면 실행된다.
    private void OnActivated(ActivateEventArgs args)
    {
        objectMaterial.color = activatedColor;
    }

    // Trigger를 놓아 Activate가 끝나면 실행된다.
    private void OnDeactivated(DeactivateEventArgs args)
    {
        // 아직 잡고 있으면 초록색으로 돌아간다.
        // 잡고 있지 않다면 Hover 여부에 따라 노란색 또는 하늘색으로 돌아간다.
        objectMaterial.color = grabInteractable.isSelected
            ? selectedColor
            : (grabInteractable.isHovered ? hoverColor : normalColor);
    }
}