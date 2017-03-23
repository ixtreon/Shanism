using Shanism.Client.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Chat
{
    class ChatBar : TextBox, IChatSource
    {
        const int MaxLineHistory = 256;


        readonly LinkedList<string> messageHistory = new LinkedList<string>();

        LinkedListNode<string> currentMsgHistoryNode;


        public event Action<string> ChatMessageSent;

        public ChatBar(IChatConsumer consumer)
        {
            ChatMessageSent += consumer.SendChatMessage;
        }

        protected override void OnUpdate(int msElapsed)
        {
            BackColor = Color.Black.SetAlpha(HasFocus ? 125 : 75);
            base.OnUpdate(msElapsed);
        }

        protected override void OnCharTyped(Keybind k, char? c)
        {
            switch (k.Key)
            {

                case Keys.Up:       // {Up}, {Down} to browse line history
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

                case Keys.Enter:    // {Enter} sends the message
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

                case Keys.Escape:       // {Esc} clears the message
                    ClearText();
                    break;
            }

            base.OnCharTyped(k, c);
        }

        private void sendMessage()
        {


            ChatMessageSent?.Invoke(Text);
        }
    }
}
