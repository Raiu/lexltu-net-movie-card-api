import { ReactElement } from 'react'

import './App.css'
import { Footer, Header } from '@components'
import { Outlet } from 'react-router';

function App(): ReactElement {
  return (
    <>
      <Header />
      <main>
        <Outlet />
      </main>
      <Footer />
    </>
  )
}

export default App;