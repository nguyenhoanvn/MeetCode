import { useState } from "react";
import LoadingOverlay from "../components/LoadingOverlay";
import useTagAdd from "../hooksAdmin/useTagAdd";

export default function TagAddPage() {
    const { tagAddForm, loading, error, success, pageLoading, arrayType, randomNumber, luckyEnable,
        handleChange, handleSubmit, enableLuckyView } = useTagAdd();

    const inputClass = `font-light border-gray-500 border p-3 rounded-lg w-100 focus:outline-none ${error ? "border-red-500 focus:border-red-500" : "focus:border-[#1e3a8a]"}`;
    return (
        <>
            {pageLoading && (
                <LoadingOverlay message="Loading tag form..." />
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                {/* Header and Add button */}
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Tags</p>
                        <p className="text-3xl capitalize text-white">Add New Tag</p>
                    </div>
                    <div className="ml-auto flex flex-col gap-5">
                        <button
                            onClick={enableLuckyView}
                            className="py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900 hover:cursor-pointer hover:bg-blue-950"
                        >
                            {luckyEnable ? (
                                <p>Disable lucky mode</p>
                            ) : (<p>I'm feeling lucky</p>)}
                            
                        </button>
                        <button
                            disabled={loading}
                            onClick={handleSubmit}
                            className="py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900 hover:cursor-pointer hover:bg-blue-950"
                        >
                            {loading ? (
                                <div className="flex items-center justify-center">
                                    <span className="material-symbols-outlined animate-spin">
                                        progress_activity
                                    </span>
                                    <p className="">Loading...</p>
                                </div>
                            ) : (<div className="flex gap-3 items-center justify-center">
                                <span className="material-symbols-outlined text-sm!">
                                    add
                                </span>
                                <p>Add tag</p>
                            </div>)}
                        </button>
                    </div>
                </div>
                {/* Error message */}
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}
                {/* Sucess message */}
                {success ? (
                    <p className="text-green-400"><span className="font-bold">Success: </span>{success}</p>
                ) : (<></>)}
                {/* Main content normal state */}
                {luckyEnable == false ? (
                    <div className="grid grid-cols-2 gap-10">
                        <div className="flex flex-col gap-3 col-span-2">
                            <p className="font-medium text-lg">Name</p>
                            <input
                                type="text"
                                name="name"
                                value={tagAddForm.name}
                                onChange={handleChange}
                                placeholder="Enter tag name..."
                                className={inputClass}
                            />
                        </div>
                    </div>) : (<></>)}

                {/* Array State */}
                {luckyEnable == true && randomNumber === 1 ?
                    (<div className="grid grid-cols-2 gap-10">
                        <div className="flex flex-col gap-3 col-span-2">
                            <p className="font-medium text-lg">Name</p>
                            {randomNumber === 1 && (
                                <div className="flex flex-col gap-3">
                                    <input
                                        type="text"
                                        name="name"
                                        value={tagAddForm.name}
                                        onChange={handleChange}
                                        placeholder="Array..."
                                        className={inputClass}
                                    />
                                    <div className="flex justify-center">
                                    <div className="grid grid-cols-10 w-fit gap-x-0 gap-y-10 ">
                                        {arrayType.map((value, i) => (
                                            <div className="flex flex-col items-center gap-3">
                                                <p className="font-medium">[{i}]</p>
                                                <p className="border px-5 py-3 border-gray-500">{value}</p>
                                            </div>
                                        ))}
                                    </div>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>) : (<></>)
                }
            </div>
        </>
    )
}
