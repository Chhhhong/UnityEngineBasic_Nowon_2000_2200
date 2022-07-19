using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Mover : MonoBehaviour
{
    private Transform _transform;
    private Camera _camera;
    // Initialization �Լ���
    // �� Ŭ������ ������Ʈ�� ������ ���ӿ�����Ʈ�� Ȱ��ȭ�Ǿ��������� ȣ���

    // �ѹ� ȣ��
    // GameObject �� Ȱ��ȭ �Ǿ��ְ�, �ش� ������Ʈ�� ��Ȱ��ȭ �Ǿ��־ ȣ���.
    // ������ Ŭ�������� �����ڷ� ����ؼ� �ʱ�ȭ�ؾ��ϴ� ���,
    // Monobehavior �� Awake ���� �ʱ�ȭ�� ��. (������ ���)
    private void Awake()
    {
        // �ν��Ͻ� ������� �ʱ�ȭ
        _transform = transform;

        // Test_Mover ������Ʈ�� ������ ���� ������Ʈ�� �������ִ�
        // Camera ������Ʈ�� ��ȯ�ϴ� �Լ�
        _camera = GetComponent<Camera>();
    }

    // ���ӿ�����Ʈ�� Ȱ��ȭ �� ������ ȣ��
    private void OnEnable()
    {

    }


    // �ѹ� ȣ��
    void Start()
    {
        // �ٸ� GameObject �� ������Ʈ �ν��Ͻ� �� �ܺ������� �ؼ� �ʱ�ȭ�ؾ��� ��
        // Awake() ���� �� �ʱ�ȭ �� �Ŀ� ���� �Ǿ�� �Ҷ�
    }

    // ��������ø��� ȣ��Ǵ� �Լ�
    // Update()���� ���� ȣ��� ���� ����
    // Update() ���� ���������� �ϸ� �ȵǴ� ������
    // ��� ���ɸ��� �ٸ��� ȣ��Ǳ⶧���� �ð��� �̵��Ÿ�/ �ӵ� ��ȭ���� ������ ��ģ��.
    void FixedUpdate()
    {
        transform.position += Vector3.up * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // ī�޶��̵� � ���
    private void LateUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one);
    }

    private void OnApplicationFocus(bool focus)
    {

    }

    private void OnApplicationPause(bool focus)
    {

    }

    

    //���� ������Ʈ�� ��Ȱ��ȭ �ɶ����� ȣ��
    private void OnDisable()
    {

    }

    // ���ӿ�����Ʈ�� �ı��ɶ� ȣ��
    private void OnDestroy()
    {

    }
}
