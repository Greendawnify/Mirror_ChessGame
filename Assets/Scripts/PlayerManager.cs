using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public List<GameObject> spawnedPieces = new List<GameObject>();
    private List<GameObject> pieces = new List<GameObject>();
    private Transform playerArea;
    private List<Transform> piecePositions = new List<Transform>();
    private BoardManager board;

    [SyncVar]
    public int playerNumber;


    void Update() { 
        
    }

    public override void OnStartClient()
    {
        
        base.OnStartClient();
        if (hasAuthority)
        {
            CmdSpawnPieces();
            
        }
        
    }

    private void Start()
    {
        if (!hasAuthority) return;

        board = GameObject.Find("GameBoard").GetComponent<BoardManager>();

        SetPieces();
        board.playerNumber += 1;
        
    }

    void SetPieces() {
        playerArea = GameObject.Find("Player" + board.playerNumber + "Area").transform;
        for (int i = 0; i < playerArea.childCount; i++)
        {
            Transform location = playerArea.GetChild(i);
            piecePositions.Add(location);
            Debug.Log(i);
            pieces[i].transform.SetParent(location);
            pieces[i].transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    [Command]
    void CmdSpawnPieces()
    {
        for(int i = 0; i< spawnedPieces.Count; i++)
        {
            GameObject piece = Instantiate(spawnedPieces[i]);
            NetworkServer.Spawn(piece, connectionToClient);
            TargetFillPieceList(piece);
            //pieces.Add(piece);
            //piece.SetActive(false);
        }
    }

    [TargetRpc]
    void TargetFillPieceList(GameObject p)
    {
        Debug.Log("called on client");
        pieces.Add(p);
    }
}
