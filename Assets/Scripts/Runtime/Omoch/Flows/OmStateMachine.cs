using System;
using System.Collections.Generic;

#nullable enable

namespace Omoch.Flows
{
    /// <summary>
    /// ステート切り替え時にパラメータを渡せるステートマシン
    /// </summary>
    public class OmStateMachine<TContext>
    {
        /// <summary>
        /// 現在のステート
        /// </summary>
        private IState? currentState;

        /// <summary>
        /// コンストラクタで渡したコンテキスト
        /// </summary>
        private readonly TContext context;
        public TContext Context => context;

        /// <summary>
        /// GetState()で生成したステートのプール場所
        /// </summary>
        private readonly Dictionary<Type, IState> statePool;

        /// <summary>
        /// State.Exit()の中でChangeState()していないかの確認用
        /// </summary>
        private bool isChangingState;

        public OmStateMachine(TContext context)
        {
            this.context = context ?? throw new ArgumentNullException();

            statePool = new();
            isChangingState = false;
        }

        /// <summary>
        /// ステートを取得する（なければ生成）
        /// </summary>
        public TState GetState<TState>()
            where TState : IState, new()
        {
            Type type = typeof(TState);
            if (statePool.TryGetValue(type, out IState pooledState))
            {
                return (TState)pooledState;
            }

            var state = new TState();
            state.StateMachine = this;
            statePool[type] = state;
            return state;
        }

        /// <summary>
        /// ステートを変更する
        /// </summary>
        public void ChangeState<TState>() where TState : State, new()
        {
            if (isChangingState)
            {
                throw new Exception("State.Exit()の中でChangeState()はできません");
            }

            isChangingState = true;
            var prevState = currentState;
            currentState = GetState<TState>();
            prevState?.Exit(currentState);
            isChangingState = false;
            currentState.Enter(prevState);
        }

        /// <summary>
        /// ステートを変更し、ステートに引数のデータを渡す
        /// </summary>
        public void ChangeState<TState, TData>(TData data)
            where TState : StateWith<TData>, new()
        {
            if (isChangingState)
            {
                throw new Exception("State.Exit()の中でChangeState()はできません");
            }

            isChangingState = true;
            var prevState = currentState;
            currentState = GetState<TState>();
            prevState?.Exit(currentState);
            isChangingState = false;
            ((StateWith<TData>)currentState).StateData = data;
            currentState.Enter(prevState);
        }

        /// <summary>
        /// 現在のステートのUpdateを呼ぶ
        /// </summary>
        public void Update()
        {
            currentState?.Update();
        }

        /// <summary>
        /// 継承して使うステート本体
        /// </summary>
        public abstract class State : IState
        {
            private OmStateMachine<TContext>? stateMachine;
            public OmStateMachine<TContext> StateMachine
            {
                get => stateMachine ?? throw new ArgumentNullException();
                set => stateMachine = value;
            }
            public TContext Context => StateMachine.Context;
            public virtual void Enter(IState? prevState) { }
            public virtual void Update() { }
            public virtual void Exit(IState? nextState) { }
        }

        /// <summary>
        /// 継承して使うステート本体
        /// ChangeState時にStateへデータを1つ持たせられる
        /// </summary>
        public abstract class StateWith<TData> : IState
        {
            private OmStateMachine<TContext>? stateMachine;
            public OmStateMachine<TContext> StateMachine
            {
                get => stateMachine ?? throw new ArgumentNullException();
                set => stateMachine = value;
            }
            public TContext Context => StateMachine.Context;
            public virtual void Enter(IState? prevState) { }
            public virtual void Update() { }
            public virtual void Exit(IState? nextState) { }

            /// <summary>
            /// ChangeState時に受け取ったデータ
            /// </summary>
            private TData? stateData;
            public TData StateData
            {
                get => stateData ?? throw new ArgumentNullException();
                set => this.stateData = value;
            }
        }

        public interface IState
        {
            public OmStateMachine<TContext> StateMachine { get; set; }
            public TContext Context { get; }
            public void Enter(IState? prevState);
            public void Update();
            public void Exit(IState? nextState);
        }
    }
}
