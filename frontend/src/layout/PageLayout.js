import "./PageLayout.css"

export default function PageLayout({children}){
    return (
        <main className="page-layout">
            {children}
        </main>
    )
}