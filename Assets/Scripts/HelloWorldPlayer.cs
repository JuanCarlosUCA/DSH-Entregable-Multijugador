using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if(IsServer){
                SubmitPositionRequestServerRpc();
            }
            else
            {
                SubmitPositionRequestOwnerRpc();
            }
        }

        [Rpc(SendTo.Server)]
        private void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
        {
            var initialPosition = GetInitialPositionOnPlane();
            transform.position = initialPosition;
            Position.Value = initialPosition;
        }


        [Rpc(SendTo.Owner)]
        private void SubmitPositionRequestOwnerRpc(RpcParams rpcParams = default)
        {
            var initialPosition = GetInitialPositionOnPlane();
            transform.position = initialPosition;
        }

        static Vector3 GetInitialPositionOnPlane()
        {
            return new Vector3(0,1f,0);
        }

        private void Update()
        {
            //que solo se aplique al prefab concreto propietario que está en la ventana
            if (IsOwner)
            {
                float hor = Input.GetAxis("Horizontal");
                float ver = Input.GetAxis("Vertical");

                Vector3 desp = new Vector3(hor, 0, ver) * 3 * Time.deltaTime;

                transform.Translate(desp);
            }
        }
    }
}