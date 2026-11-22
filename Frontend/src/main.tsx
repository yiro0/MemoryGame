import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {BrowserRouter} from "react-router-dom";
import {ErrorBoundary} from './components/ErrorBoundary';

/**
 * Application entry point: mounts <App/> inside StrictMode, a global error boundary and the router.
 */
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ErrorBoundary>
            <BrowserRouter>
                <App/>
            </BrowserRouter>
        </ErrorBoundary>
    </StrictMode>,
)
