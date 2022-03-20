using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    [SerializeField] private OVRHand handRH;
    [SerializeField] private OVRHand handLH;

    [SerializeField] private GameObject ui_01;
    [SerializeField] private GameObject ui_02;
    [SerializeField] private GameObject ui_03;
    [SerializeField] private GameObject ui_04;
    [SerializeField] private GameObject ui_05;
    [SerializeField] private GameObject ui_06;
    [SerializeField] private Material _upUI;
    [SerializeField] private Material _downUI;

    [SerializeField] private GameObject _hatenaHoloPrefab;
    [SerializeField] private GameObject _blockHoloPrefab;
    [SerializeField] private GameObject _hatenaPrefab;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private float _posy;
    [SerializeField] private float _posz;
    [SerializeField] private float _posx;
    private GameObject _newObj;
    private Transform _newTransform;
    [SerializeField] private GameObject _moveObj;
    private Vector3 _pos;
    private int blockCt; //�u���b�N�������J�E���g
    private RaycastHit hit;

    [SerializeField] private Text _text;

    public string conName;
    public BoxCollider connBox;

    private Ray ray;

    private float waitTime;

    private bool isUsual;       // �ʏ펞����
    
    private bool isHover;       // UI�z�o�[���
    private bool isMakeHatena;  // �͂Ăȃu���b�N�쐬���j���[�N��
    private bool isMakeblock;   // �ʏ�u���b�N�쐬���j���[�N��
    private bool isConn;        // �u���b�N�쐬���̍쐬�敽�ʃz�o�[���
    
    public bool isDelete;
    
    public bool isMove;            // �ړ����j���[�N��
    public bool isBlockHover;      // �ړ����j���[�N�����̃u���b�N�z�o�[���
    private bool isMoveBlockSelect; // �ړ����s��

    public Transform anchor;
    public AudioClip sound;
    private float maxDistance = 100;
    private LineRenderer lineRenderer;
    private string _hitName;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        isUsual = true;
    }

    void Update()
    {
        #region normal
        //�ʏ펞
        if (isUsual == true)
        {
            if (OVRInput.IsControllerConnected(OVRInput.Controller.Touch) == true)
            {
                lineRenderer.enabled = true;
                
                ray = new Ray(anchor.position, anchor.forward);
                //Ray�̊J�n�_�ݒ�
                lineRenderer.SetPosition(0, ray.origin);

                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    GameObject target = hit.collider.gameObject;
                    //Ray�̏I�_�ݒ�
                    lineRenderer.SetPosition(1, hit.point);

                    //UI�z�o�[���
                    if (target.CompareTag("UI"))
                    {
                        isHover = true;
                        // ���ʉ���炷�B
                        if (target.transform.localPosition == Vector3.zero)
                            AudioSource.PlayClipAtPoint(sound, transform.position);
                        target.transform.localPosition = new Vector3(0f, 0.02f, 0f);
                        target.GetComponent<Renderer>().material = _upUI;
                        positionReset(target);
                    }
                    else { isHover = false; }

                    //�z�o�[��ԂŉE�g���K�[ON
                    if (isHover && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                    {
                        TriggerEnter(target);
                    }
                }
                else
                {
                    positionReset();
                    lineRenderer.SetPosition(1, ray.origin + (ray.direction * maxDistance));
                }
            }else
            {
                lineRenderer.enabled = false;
            }
        }
        #endregion

        #region Make Block
        //�u���b�N�쐬
        if (isMakeHatena == true || isMakeblock == true || isMoveBlockSelect == true)
        {
            Vector3 _pos3 = anchor.position;
            _pos3.y += _posy;
            _pos3.z += _posz;
            if(isConn == false) _newObj.transform.position = _pos3;
            if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp)) _posy += 0.01f;
            if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown)) _posy -= 0.01f;
            if (OVRInput.Get(OVRInput.RawButton.RThumbstickRight)) _posz += 0.01f;
            if (OVRInput.Get(OVRInput.RawButton.RThumbstickLeft)) _posz -= 0.01f;

            // UI�I���Ɠ����Ƀu���b�N�ʒu���m�肳��Ȃ��悤�ɂ���
            waitTime += Time.deltaTime;

            // �E�O���b�v�g���K�[ON��Ray��L����
            if (waitTime > 1.0f && OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                lineRenderer.enabled = true;
                ray = new Ray(anchor.position, anchor.forward);
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, ray.origin + (ray.direction * maxDistance));
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    // �Ώە���Ray���~�߂�
                    lineRenderer.SetPosition(1, hit.point);
                    GameObject conn = hit.collider.gameObject;
                    // �u���b�N�T�C�h���ʂ��z�o�[��
                    if (conn.CompareTag("Connect"))
                    {
                        isConn = true;
                        Vector3 newPos = conn.transform.position;
                        newPos.y += 0.05f;
                        _newObj.transform.position = newPos;
                        conName = conn.transform.parent.parent.gameObject.name + "_" + conn.name;
                        // �z�o�[��ԂŃg���K�[ON
                        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                        {
                            // �u���b�N���쐬
                            if (isMakeHatena == true || isMakeblock == true)
                            {
                                MakeBlock(newPos, conn);
                                blockCt += 1;
                            }
                            else if (isMoveBlockSelect == true)
                            {
                                _moveObj.transform.position = newPos;
                                _moveObj.SetActive(true);
                            }
                            // �p�����[�^�����Z�b�g
                            iniPalam();
                        }

                    }
                    else
                    {
                        isConn = false;
                        conName = "";
                    }
                }
                else
                {
                    isConn = false;
                    conName = "";
                }
            }
            else
            {
                lineRenderer.enabled = false;
                isConn = false;
                conName = "";
            }

            if (waitTime > 1.0f && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                // �u���b�N���쐬
                if (isMakeHatena == true || isMakeblock == true) MakeBlock(_newObj.transform.position);
                // �u���b�N���ړ�������
                else if (isMoveBlockSelect == true)
                {
                    _moveObj.transform.position = _newObj.transform.position;
                    _moveObj.SetActive(true);
                }                
                iniPalam();
                blockCt += 1;
            }

            // �쐬�L�����Z��
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                if (_moveObj != null) _moveObj.SetActive(true);
                lineRenderer.enabled = true;
                iniPalam();
            }
        }
        #endregion

        #region Move Block Select
        if (isMove == true || isDelete == true)
        {
            ray = new Ray(anchor.position, anchor.forward);
            //Ray�̊J�n�_�ݒ�
            lineRenderer.SetPosition(0, ray.origin);

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                GameObject moveTarget = hit.collider.gameObject;
                //Ray�̏I�_�ݒ�
                lineRenderer.SetPosition(1, hit.point);

                //�ړ��u���b�N���I�����
                if (isMoveBlockSelect == false)
                {
                    //�u���b�N�z�o�[��Ԕ���
                    if (moveTarget.CompareTag("Box"))
                    {
                        // ���ʉ���炷�B
                        if (isBlockHover == false)
                        {
                            isBlockHover = true;
                            if (_hitName != moveTarget.name)
                            {
                                AudioSource.PlayClipAtPoint(sound, transform.position);
                            }
                            _hitName = moveTarget.name;
                        }

                        //�ړ��Ώۃu���b�N�I�𔻒�
                        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                        {
                            _moveObj = moveTarget.transform.parent.gameObject;
                            BlockEdit(moveTarget.transform.parent.gameObject);
                        }
                    }
                    else 
                    {
                        Debug.Log("not hover");
                        isBlockHover = false;
                    }
                }
            }
            else
            {
                lineRenderer.SetPosition(1, ray.origin + (ray.direction * maxDistance));
            }
            
            if(OVRInput.GetDown(OVRInput.RawButton.B))
            {
                iniPalam();
            }
        }
        #endregion
    }

    #region UI Position Reset
    //UI�ʒu���Z�b�g
    void positionReset(GameObject target = null)
    {
        if (target == null)
        {
            ui_01.transform.localPosition = new Vector3(0, 0, 0);
            ui_01.GetComponent<Renderer>().material = _downUI;
            ui_02.transform.localPosition = new Vector3(0, 0, 0);
            ui_02.GetComponent<Renderer>().material = _downUI;
            ui_03.transform.localPosition = new Vector3(0, 0, 0);
            ui_03.GetComponent<Renderer>().material = _downUI;
            ui_04.transform.localPosition = new Vector3(0, 0, 0);
            ui_04.GetComponent<Renderer>().material = _downUI;
            ui_05.transform.localPosition = new Vector3(0, 0, 0);
            ui_05.GetComponent<Renderer>().material = _downUI;
            ui_06.transform.localPosition = new Vector3(0, 0, 0);
            ui_06.GetComponent<Renderer>().material = _downUI;
        }
        else
        {
            if (ui_01 != target)
            {
                ui_01.transform.localPosition = new Vector3(0, 0, 0);
                ui_01.GetComponent<Renderer>().material = _downUI;
            }
            if (ui_02 != target)
            {
                ui_02.transform.localPosition = new Vector3(0, 0, 0);
                ui_02.GetComponent<Renderer>().material = _downUI;
            }
            if (ui_03 != target)
            {
                ui_03.transform.localPosition = new Vector3(0, 0, 0);
                ui_03.GetComponent<Renderer>().material = _downUI;
            }

            if (ui_04 != target)
            {
                ui_04.transform.localPosition = new Vector3(0, 0, 0);
                ui_04.GetComponent<Renderer>().material = _downUI;
            }
            if (ui_05 != target)
            {
                ui_05.transform.localPosition = new Vector3(0, 0, 0);
                ui_05.GetComponent<Renderer>().material = _downUI;
            }
            if (ui_06 != target)
            {
                ui_06.transform.localPosition = new Vector3(0, 0, 0);
                ui_06.GetComponent<Renderer>().material = _downUI;
            }
        }
    }
    #endregion

    #region Make Block Palameter Reset
    //�u���b�N�쐬��ɕK�v�ȕϐ��̃��Z�b�g
    void iniPalam()
    {
        isMakeHatena = false;
        isMakeblock = false;
        isMove = false;
        isMoveBlockSelect = false;
        isDelete = false;
        isConn = false;
        isUsual = true;
        _moveObj = null;
        conName = "";
        Destroy(_newObj);
        waitTime = 0;
        _posy = 0.08f;
        _posz = 0.08f;
        lineRenderer.enabled = true;
        _text.text = "";
    }
    #endregion

    #region Make Block Function
    //�u���b�N���쐬����
    void MakeBlock(Vector3 _makePosition, GameObject _conn = null)
    {
        BlockObject blockObj;

        //�͂Ăȃu���b�N���쐬
        if (isMakeHatena == true && isMakeblock == false)
        {
            var _putHatenaObject = Instantiate(_hatenaPrefab, _makePosition, Quaternion.identity);
            _putHatenaObject.name += ("_" + blockCt);
            try
            {
                BlockHit _blockhit = _putHatenaObject.GetComponentInChildren<BlockHit>();
                _blockhit._ovrLeftHand = handLH;
                _blockhit._ovrRightHand = handRH;
            }
            catch { }
            
            if (_conn != null)
            {
                blockObj = _putHatenaObject.GetComponent<BlockObject>();
                blockObj.connCollider = _conn.GetComponent<BoxCollider>();
            }
        }
        //�ʏ�u���b�N���쐬
        if (isMakeHatena == false && isMakeblock == true)
        {
            var _putBlockObject = Instantiate(_blockPrefab, _makePosition, Quaternion.identity);
            _putBlockObject.name += ("_" + blockCt);
            try 
            {
                BlockHit _blockhit = _putBlockObject.GetComponentInChildren<BlockHit>();
                _blockhit._ovrLeftHand = handLH;
                _blockhit._ovrRightHand = handRH;
            }
            catch { }
                
            if (_conn != null)
            {
                blockObj = _putBlockObject.GetComponent<BlockObject>();
                blockObj.connCollider = _conn.GetComponent<BoxCollider>();
            }
        }
    }
    #endregion

    #region Move OR Delete Block
    void BlockEdit(GameObject _moveObject)
    {
        // ���u���b�N�̉��ɗׂ̃u���b�N�̕��ʂ�����ꍇ�͗L����
        try
        {
            var setblock = _moveObject.GetComponent<BlockObject>();
            _moveObject.SetActive(false);
            setblock.connCollider.enabled = true;
        }
        catch (Exception e)
        {
            Debug.Log("��O���L���b�`���܂���");
            Debug.Log(e);
        }
        finally
        {
            _moveObject.SetActive(false);
        }

        if (isMove == true)
        {
            // �z���O�����p�̃u���b�N�ʒu����
            _newTransform = anchor;
            _pos = anchor.position;
            _pos.y += 0.1f;
            _pos.z += 0.1f;
            _newTransform.position = _pos;
            lineRenderer.enabled = false;

            //�z���O������Ԃ̃��f�����C���X�^���X��
            if (_moveObject.name.Contains("HatenaBlock_low"))
                _newObj = Instantiate(_hatenaHoloPrefab, _newTransform.position, Quaternion.identity);
            else if (_moveObject.name.Contains("normalBlock_low"))
                _newObj = Instantiate(_blockHoloPrefab, _newTransform.position, Quaternion.identity);

            isMoveBlockSelect = true;
            isMove = false;
        }
        else if (isDelete == true)
        {
            Destroy(_moveObj);
            iniPalam();
        }
    }


    #endregion

    //Trigger ON����Hover���Ă���UI�ɂ���ď�����ς���
    void TriggerEnter(GameObject obj = null)
    {
        switch (obj.name)
        {
            //�͂Ăȃu���b�N�I��
            case "UI_01":
                isMakeHatena = true;

                _newTransform = anchor;
                _pos = anchor.position;
                _pos.y += 0.1f;
                _pos.z += 0.1f;
                _newTransform.position = _pos;
                isUsual = false;
                lineRenderer.enabled = false;

                _text.text = "MAKE HATENA BLOCK";
                _text.color = new Color(1.0f, 1.0f, 0f);

                //�z���O������Ԃ̃��f�����C���X�^���X��
                _newObj = Instantiate(_hatenaHoloPrefab, _newTransform.position, Quaternion.identity);
                break;

            //�u���b�N�I��
            case "UI_02":
                isMakeblock = true;

                _newTransform = anchor;
                _pos = anchor.position;
                _pos.y += 0.1f;
                _pos.z += 0.1f;
                _newTransform.position = _pos;
                isUsual = false;
                lineRenderer.enabled = false;

                _text.text = "MAKE BLOCK";
                _text.color = new Color(1.0f, 1.0f, 0f);

                //�z���O������Ԃ̃��f�����C���X�^���X��
                _newObj = Instantiate(_blockHoloPrefab, _newTransform.position, Quaternion.identity);
                break;

            case "UI_05":
                isDelete = true;
                _text.text = "DELETE BLOCK";
                _text.color = new Color(1.0f, 0f, 0f);
                isUsual = false;
                break;

            case "UI_06":
                isMove = true;
                _text.text = "MOVE BLOCK";
                _text.color = new Color(1.0f, 1.0f, 0f);
                isUsual = false;
                break;

            default:

                break;

        }
    }

}
