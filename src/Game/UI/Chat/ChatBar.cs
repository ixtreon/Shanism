using Shanism.Client.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client.UI.Chat
{
    class ChatBar : TextBar, IChatSource
    {
        const int MaxLineHistory = 256;


        readonly LinkedList<string> messageHistory = new LinkedList<string>();

        LinkedListNode<string> currentMsgHistoryNode;


        public event Action<string> ChatMessageReceived;

        public ChatBar(IChatConsumer consumer)
        {
            ChatMessageReceived += consumer.SendChatMessage;
        }

        public override void Update(int msElapsed)
        {
            BackColor = UiColors.ControlBackground.Brighten(IsFocusControl ? 0 : 25);
            base.Update(msElapsed);
        }
        protected override void OnCharInput(KeyboardArgs e)
        {
            switch (e.Key)
            {

                // {Up}, {Down} to browse line history
                case Keys.Up:       
                    if (messageHistory.Any())
                    {
                        if (currentMsgHistoryNode == null)
                            currentMsgHistoryNode = messageHistory.First;
                        else
                            currentMsgHistoryNode = currentMsgHistoryNode.Next;

                        Text = currentMsgHistoryNode?.Value ?? string.Empty;
                        SelectAllText();
                    }
                    break;

                case Keys.Down:
                    if (messageHistory.Any())
                    {
                        if (currentMsgHistoryNode == null)
                            currentMsgHistoryNode = messageHistory.Last;
                        else
                            currentMsgHistoryNode = currentMsgHistoryNode.Previous;

                        Text = currentMsgHistoryNode?.Value ?? string.Empty;
                        SelectAllText();
                    }
                    break;

                // {Enter} to send a message
                case Keys.Enter:    

                    //add to line history
                    if (!string.Equals(messageHistory.First?.Value, Text))
                    {
                        messageHistory.AddFirst(Text);
                        if (messageHistory.Count > MaxLineHistory)
                            messageHistory.RemoveLast();
                    }

                    //send the message
                    if (!string.IsNullOrEmpty(Text))
                    {
                        sendMessage();
                        currentMsgHistoryNode = null;
                        ClearText();
                    }

                    ClearFocus();
                    break;

                // {Esc} to close the chat bar
                case Keys.Escape:       
                    ClearText();
                    ClearFocus();
                    break;
            }

            base.OnCharInput(e);
        }

        void sendMessage()
        {
            ChatMessageReceived?.Invoke(Text);
        }
    }
}
