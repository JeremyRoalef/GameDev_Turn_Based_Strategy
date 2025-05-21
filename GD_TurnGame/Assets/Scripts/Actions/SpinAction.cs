using UnityEngine;

public class SpinAction : BaseAction
{
    [SerializeField]
    float spinSpeed = 360f;

    float spinAmount = 0;
    bool isSpinning;

    private void Update()
    {
        if (!isActive) return;

        if (isSpinning)
        {
            spinAmount += spinSpeed * Time.deltaTime;

            transform.eulerAngles += new Vector3(
                0,
                spinSpeed * Time.deltaTime,
                0
                );

            if (spinAmount >= 360f)
            {
                isSpinning = false;
                isActive = false;
                spinAmount = 0;
            }
        }
    }

    public void Spin()
    {
        isSpinning = true;
        isActive = true;
    }
}
