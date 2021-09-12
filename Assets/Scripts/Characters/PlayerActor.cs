using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerActor : MonoBehaviour, InputActions.IGameActions {
  [Header("Character Input Values")]
  public Vector2 move;
  public bool turnEnd;

  public void OnMove(InputAction.CallbackContext context) {
    MoveInput(context.ReadValue<Vector2>());
  }

  public void OnTurnEnd(InputAction.CallbackContext context) {
    TurnEndInput(context.ReadValueAsButton());
  }

  public void OnGameEnd(InputAction.CallbackContext context) {
    Application.Quit();
  }

  public void MoveInput(Vector2 newMoveDirection) {
    move = newMoveDirection;
  }

  private void TurnEndInput(bool newTurnEndState) {
    turnEnd = newTurnEndState;
  }
}
