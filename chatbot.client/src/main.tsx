import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import { Provider } from 'react-redux'
import GeneralStore from './store/GeneralStore'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={GeneralStore}>
      <App />
    </Provider>
  </React.StrictMode>,
)