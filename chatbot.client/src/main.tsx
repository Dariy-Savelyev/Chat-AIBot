import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import { Provider } from 'react-redux'
import ChatStore from './store/ChatStore'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={ChatStore}>
      <App />
    </Provider>
  </React.StrictMode>,
)