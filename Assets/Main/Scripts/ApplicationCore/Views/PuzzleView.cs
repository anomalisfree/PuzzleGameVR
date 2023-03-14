using System.Collections.Generic;
using Autohand;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class PuzzleView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> puzzlePieces;

        private void Start()
        {
            foreach (var puzzlePiece in puzzlePieces)
            {
                puzzlePiece.GetComponent<GrabbableBase>().body =  puzzlePiece.GetComponent<Rigidbody>();
                //
                // if (puzzlePiece.GetComponent<RealtimeTransform>().isUnownedSelf || puzzlePiece.GetComponent<RealtimeView>().isUnownedSelf)
                // {
                //     puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                //     puzzlePiece.GetComponent<RealtimeTransform>().RequestOwnership();
                // }
            }
        }
    }
}
